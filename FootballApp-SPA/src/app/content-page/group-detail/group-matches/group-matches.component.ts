import { Component, OnInit, Input } from '@angular/core';
import { MatchPlayed } from 'src/app/_models/matchPlayed.enum';

@Component({
  selector: 'app-group-matches',
  templateUrl: './group-matches.component.html',
  styleUrls: ['./group-matches.component.css']
})
export class GroupMatchesComponent implements OnInit {
  @Input() mode: MatchPlayed;
  MatchPlayed = MatchPlayed;
  constructor() { }

  ngOnInit() {
  }

}
