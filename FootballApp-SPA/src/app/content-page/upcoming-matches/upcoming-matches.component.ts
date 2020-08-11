import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';
import { MatchService } from 'src/app/_services/match.service';
import { MatchPlayed } from 'src/app/_models/matchPlayed.enum';

@Component({
  selector: 'app-upcoming-matches',
  templateUrl: './upcoming-matches.component.html',
  styleUrls: ['./upcoming-matches.component.css'],
})
export class UpcomingMatchesComponent implements OnInit {
  titleToDisplay: string;
  appliedMatches: any;
  user: any;
  MatchPlayed = MatchPlayed;

  constructor(private route: ActivatedRoute,
              private matchService: MatchService) {}

  ngOnInit() {
    this.user = JSON.parse(localStorage.getItem('user'));
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data['title'];
    });
    this.matchService.getUpcomingMatchesForUser(this.user.id).subscribe(
      (res: any) => {
        this.appliedMatches = res;
      }
    );
  }
}
