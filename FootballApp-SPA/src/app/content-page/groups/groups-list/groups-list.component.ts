import { Group } from "./../../../_models/group";
import { Component, OnInit } from "@angular/core";
import { GroupStatus } from 'src/app/_models/GroupStatus.enum';

@Component({
  selector: "app-groups-list",
  templateUrl: "./groups-list.component.html",
  styleUrls: ["./groups-list.component.css"]
})
export class GroupsListComponent implements OnInit {
  groups: Group[] = [
    {
      name: "Jelovac",
      description: "Fudbal na Kalvariji",
      numberOfMembers: 23,
      dateCreated: new Date(),
      member: true,
    },
    {
      name: "Altina",
      description: "Vesela druzina sa altine",
      numberOfMembers: 18,
      dateCreated: new Date(),
      member: false,
    },
    {
      name: "Rakovica",
      description: "Dzomba i drugari",
      numberOfMembers: 16,
      dateCreated: new Date(),
      member: false,
    }
  ];

  constructor() {}

  ngOnInit() {}

  sayHello(text){
    console.log(text);
  }
}
