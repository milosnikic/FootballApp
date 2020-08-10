import { Component, OnInit, ViewChild } from '@angular/core';
import { Group } from 'src/app/_models/group';
import { ActivatedRoute, Data, ParamMap } from '@angular/router';
import { MatchPlayed } from 'src/app/_models/matchPlayed.enum';
import { MatTabGroup } from '@angular/material';
import { User } from 'src/app/_models/user';
import { GroupsService } from 'src/app/_services/groups.service';
import { MatchService } from 'src/app/_services/match.service';
import { MembershipStatus } from 'src/app/_models/MembershipStatus.enum';
import { Role } from 'src/app/_models/role.enum';

@Component({
  selector: 'app-group-detail',
  templateUrl: './group-detail.component.html',
  styleUrls: ['./group-detail.component.css']
})
export class GroupDetailComponent implements OnInit {
  user: User;
  titleToDisplay: string;
  MatchPlayed = MatchPlayed;
  @ViewChild('groupTabs', { static: true }) tabs: MatTabGroup;
  pendingRequests: User[];
  groupMembers: User[];
  playedMatches: any[];
  upcomingMatches: any[];
  groupId: number;
  group: any;
  // TODO: add membership model
  membershipInfo: any;
  MembershipStatus = MembershipStatus;
  Role = Role;

  constructor(private route: ActivatedRoute,
              private groupService: GroupsService,
              private matchService: MatchService) { }

  ngOnInit() {
    this.route.paramMap.subscribe((data: ParamMap) => {
      this.groupId = +data.get('groupId');
    });
    this.user = JSON.parse(localStorage.getItem('user'));
    this.groupService.getDetailGroupInformation(this.groupId, this.user.id).subscribe(
      (res) => {
        this.group = res;
        this.titleToDisplay = this.group.name;
        this.pendingRequests = this.group.pendingRequests;
        this.groupMembers = this.group.members;
      }
    );
    this.groupService.getMembershipInformation(this.groupId, this.user.id).subscribe(
      (res: any) => {
        console.log(res);
        this.membershipInfo = res;
      }
    );
    this.matchService.getUpcomingMatches(this.groupId).subscribe(
      (res: any) => {
        this.upcomingMatches = res;
      }
    );
  }

  changeTab(number) {
    this.tabs.selectedIndex = number;
  }
}
