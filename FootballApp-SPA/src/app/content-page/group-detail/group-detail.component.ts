import { Component, OnInit } from '@angular/core';
import { Group } from 'src/app/_models/group';
import { ActivatedRoute, Data } from '@angular/router';
import { MatchPlayed } from 'src/app/_models/matchPlayed.enum';

@Component({
  selector: 'app-group-detail',
  templateUrl: './group-detail.component.html',
  styleUrls: ['./group-detail.component.css']
})
export class GroupDetailComponent implements OnInit {
  titleToDisplay: string;
  MatchPlayed = MatchPlayed;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data['title'];
    })
  }

}
