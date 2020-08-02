import { Group } from './../../../_models/group';
import { Component, OnInit, Input } from '@angular/core';
import { GroupStatus } from 'src/app/_models/GroupStatus.enum';
import { GroupsService } from 'src/app/_services/groups.service';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css'],
})
export class GroupsListComponent implements OnInit {
  @Input() groups: Group[] = [];
  @Input() mode: GroupStatus;
  GroupStatus = GroupStatus;

  constructor(private groupService: GroupsService) {}

  ngOnInit() {}
}
