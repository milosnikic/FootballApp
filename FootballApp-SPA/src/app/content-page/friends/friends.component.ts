import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class FriendsComponent implements OnInit {
 
  titleToDisplay: string;
  displayUserDetail: boolean = false;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data
      .subscribe(
        (data:Data) => {
          this.titleToDisplay = data['title'];
        }
      );
  }

  showUserDetails(event){
    this.displayUserDetail = event;
  }
}
