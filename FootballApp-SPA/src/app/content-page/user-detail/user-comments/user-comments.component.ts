import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { CommentsService } from 'src/app/_services/comments.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { NotifyService } from 'src/app/_services/notify.service';

@Component({
  selector: 'app-user-comments',
  templateUrl: './user-comments.component.html',
  styleUrls: ['./user-comments.component.css'],
})
export class UserCommentsComponent implements OnInit {
  commentForm: FormGroup;
  @Input() comments: any[];
  @Output() profile = new EventEmitter<number>();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private commentsService: CommentsService,
    private localStorage: LocalStorageService,
    private notifyService: NotifyService
  ) {}

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    this.commentForm = this.fb.group({
      comment: ['', Validators.required],
    });
  }

  postComment() {
    const commenterId = JSON.parse(this.localStorage.get('user')).id;
    let commentedId = commenterId;
    this.route.paramMap.subscribe((res: ParamMap) => {
      if (res.get('userId')) {
        // We are on apps/users/{userId} route
        commentedId = +res.get('userId');
      }
    });
    const content = this.commentForm.get('comment').value;
    this.commentsService
      .postComment(
        {
          commenterId,
          commentedId,
          content,
        },
        commenterId
      )
      .subscribe(
        (res: any) => {
          if (res.key) {
            this.notifyService.showSuccess('Comment posted successfully!');
            this.commentsService
              .getCommentsForUser(commentedId)
              .subscribe((res: any) => {
                this.comments = res;
              });
          } else {
            this.notifyService.showError(res.value);
          }
        },
        (err) => {
          this.notifyService.showError(err);
        }
      );
  }

  changeToProfileTab() {
    this.profile.emit(0);
  }
}
