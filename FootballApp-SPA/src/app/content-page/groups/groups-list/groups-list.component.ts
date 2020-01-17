import { Group } from "./../../../_models/group";
import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-groups-list",
  templateUrl: "./groups-list.component.html",
  styleUrls: ["./groups-list.component.css"]
})
export class GroupsListComponent implements OnInit {
  member = true;
  groups: Group[] = [
    {
      name: "Jelovac",
      description: "Fudbal na Kalvariji",
      numberOfMembers: 23,
      dateCreated: new Date()
    },
    {
      name: "Altina",
      description: "Vesela druzina sa altine",
      numberOfMembers: 18,
      dateCreated: new Date()
    },
    {
      name: "Rakovica",
      description: "Dzomba i drugari",
      numberOfMembers: 16,
      dateCreated: new Date()
    }
  ];

  constructor() {}

  ngOnInit() {}

  sayHello(text){
    console.log(text);
  }
}
