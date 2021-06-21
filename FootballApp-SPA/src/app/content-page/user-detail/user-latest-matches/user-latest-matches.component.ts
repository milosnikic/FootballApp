import { Component, OnInit } from "@angular/core";
import { LocalStorageService } from "src/app/_services/local-storage.service";
import { MatchService } from "src/app/_services/match.service";

@Component({
  selector: "app-user-latest-matches",
  templateUrl: "./user-latest-matches.component.html",
  styleUrls: ["./user-latest-matches.component.css"],
})
export class UserLatestMatchesComponent implements OnInit {
  averageGoals: number;
  averageAssists: number;
  averageRating: number;
  firstMatches: any;
  thirdMatch: any;
  lastMatches: any;
  userId: number;

  constructor(
    private matchService: MatchService,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit() {
    this.userId = this.localStorageService.getCurrentLoggedInUserId();
    this.matchService.getLatestFiveMatchesForUser(this.userId).subscribe(
      (res) => {
        if (res.length) {
          this.firstMatches = res.slice(0, 2);
          if (res.length > 2) {
            this.thirdMatch = res[2];
            if (res.length > 3) {
              this.lastMatches = res.slice(2, 2);
            }
          }
        }
      },
      (err) => {}
    );
  }

  public getResultStyle(result: string) {
    switch (result) {
      case "WIN":
        return { color: "green" };
      case "LOSE":
        return { color: "red" };
      case "DRAW":
        return { color: "yellow" };
    }
  }
}
