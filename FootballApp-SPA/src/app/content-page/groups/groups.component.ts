import { Component, OnInit, ViewEncapsulation, ViewChild } from "@angular/core";
import { ActivatedRoute, Data } from "@angular/router";
import { MatTabGroup } from '@angular/material';
import { GroupsService } from 'src/app/_services/groups.service';
import { Observable } from 'rxjs';
import { Group } from 'src/app/_models/group';
import { filter, first } from 'rxjs/operators';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { MembershipStatus } from 'src/app/_models/MembershipStatus.enum';

@Component({
  selector: "app-groups",
  templateUrl: "./groups.component.html",
  styleUrls: ["./groups.component.css"],
  encapsulation: ViewEncapsulation.None
})
export class GroupsComponent implements OnInit {
  userId: number;
  titleToDisplay: string;
  @ViewChild('tabs', {static : true}) tabs: MatTabGroup;
  MembershipStatus = MembershipStatus;

  allGroups$: Observable<Group[]>;
  usersGroups$: Observable<Group[]>;
  usersCreatedGroups$: Observable<Group[]>;

  constructor(private route: ActivatedRoute,
              private groupService: GroupsService,
              private localStorage: LocalStorageService) {}
  
  ngOnInit() {
    this.userId = JSON.parse(this.localStorage.get('user')).id;
    this.groupService.getAllGroups(this.userId);
    this.groupService.getUsersGroups(this.userId);
    this.groupService.getUsersCreatedGroups(this.userId);
    this.route.data.subscribe((data: Data) => {
        this.titleToDisplay = data["title"];
      });
    this.allGroups$ = this.groupService.allGroups$;
    this.usersGroups$ = this.groupService.usersGroups$;
    this.usersCreatedGroups$ = this.groupService.usersCreatedGroups$;
  }

  changeTab(event){
    this.tabs.selectedIndex = event;
  }
}
