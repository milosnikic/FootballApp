import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';

@Component({
  selector: 'app-available-matches',
  templateUrl: './available-matches.component.html',
  styleUrls: ['./available-matches.component.css']
})
export class AvailableMatchesComponent implements OnInit {

  
  titleToDisplay: string;
  
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data
      .subscribe(
        (data:Data) => {
          this.titleToDisplay = data['title'];
        }
      );
  }
}
