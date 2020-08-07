import { Component, OnInit, Input } from '@angular/core';
import { Group } from 'src/app/_models/group';

@Component({
  selector: 'app-group-about',
  templateUrl: './group-about.component.html',
  styleUrls: ['./group-about.component.css']
})
export class GroupAboutComponent implements OnInit {
  @Input() group: any;

  constructor() { }

  ngOnInit() {
  }

}
