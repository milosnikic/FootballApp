import { Component, OnInit, ViewEncapsulation, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css'],
})
export class FriendsListComponent implements OnInit {
  @Output() event = new EventEmitter<boolean>();
  constructor() { }

  ngOnInit() {
  }

  onShowDetails(){
    this.event.emit(true);
  }
}
