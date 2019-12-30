import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {

  itemToDisplay = 'dashboard';
  constructor() { }

  ngOnInit() {
  }

  switchTo(event){
    this.itemToDisplay = event;
  }

}
