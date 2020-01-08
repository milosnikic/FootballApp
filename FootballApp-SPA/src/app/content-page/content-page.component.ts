import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Data } from "@angular/router";

@Component({
  selector: "app-content-page",
  templateUrl: "./content-page.component.html",
  styleUrls: ["./content-page.component.css"]
})
export class ContentPageComponent implements OnInit {
  titleToDisplay: string;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    // this.titleToDisplay = this.route.snapshot.data['title'];
    
  }
}
