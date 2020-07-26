import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Data } from "@angular/router";

@Component({
  selector: "app-groups",
  templateUrl: "./groups.component.html",
  styleUrls: ["./groups.component.css"],
  encapsulation: ViewEncapsulation.None
})
export class GroupsComponent implements OnInit {
  titleToDisplay: string;

  constructor(private route: ActivatedRoute) {}
  
  ngOnInit() {
    this.route.data.subscribe((data: Data) => {
        this.titleToDisplay = data["title"];
      });
  }
}
