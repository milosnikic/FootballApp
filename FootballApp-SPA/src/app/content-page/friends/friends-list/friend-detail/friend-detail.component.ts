import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-friend-detail",
  templateUrl: "./friend-detail.component.html",
  styleUrls: ["./friend-detail.component.css"]
})
export class FriendDetailComponent implements OnInit {
  @Input()
  buttonDisplay: string;

  constructor() {}

  ngOnInit() {
  }

  addFriend(){
    this.buttonDisplay = "requestSent";
  }


  remove() {
    this.buttonDisplay = "addFriend";
  }
}
