import { Data, ActivatedRoute } from "@angular/router";
import { Component, OnInit } from "@angular/core";
import { MatchService } from "src/app/_services/match.service";
import { LocalStorageService } from "src/app/_services/local-storage.service";
import { NotifyService } from "src/app/_services/notify.service";

@Component({
  selector: "app-match-history",
  templateUrl: "./match-history.component.html",
  styleUrls: ["./match-history.component.css"],
})
export class MatchHistoryComponent implements OnInit {
  titleToDisplay: string;
  matchHistory = [];
  userId: number;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private localStorageService: LocalStorageService,
    private notify: NotifyService
  ) {}

  ngOnInit() {
    this.userId = this.localStorageService.getCurrentLoggedInUserId();
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data["title"];
    });

    this.matchService.getMatchHistoryForUser(this.userId).subscribe(
      (res) => {
        this.matchHistory = res;
      },
      (err) => {
        this.notify.showError(err);
      }
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
