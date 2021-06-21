import { HttpParams } from "@angular/common/http";
import { Component, Input, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormGroupDirective,
  NgForm,
  Validators,
} from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Statistics } from "src/app/_models/statistics";
import { Team } from "src/app/_models/team";
import { MatchService } from "src/app/_services/match.service";
import { NotifyService } from "src/app/_services/notify.service";

export class PlayerStatisticsErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(
      control &&
      control.invalid &&
      (control.dirty || control.touched || isSubmitted)
    );
  }
}

@Component({
  selector: "app-organize-users",
  templateUrl: "./organize-users.component.html",
  styleUrls: ["./organize-users.component.css"],
})
export class OrganizeUsersComponent implements OnInit {
  @Input() displayMode: DisplayMode;
  DisplayMode = DisplayMode;
  @Input() users: any = [];
  selectedPlayer: any;
  playerToRemoveOrSwitch: any;
  switchPlayer: any;
  isSwitchActivated: boolean = false;
  homeTeam = new Team("team1");
  awayTeam = new Team("team2");
  organizedPlayersCount: number = 0;
  playerStatisticsForm: FormGroup;
  isMatchOrganized: boolean = false;
  private matchdayId: number;

  matcher = new PlayerStatisticsErrorStateMatcher();

  private defaultStatistics = {
    goals: 0,
    assists: 0,
    minutesPlayed: 60,
  };

  constructor(
    private fb: FormBuilder,
    private matchService: MatchService,
    private route: ActivatedRoute,
    private notify: NotifyService
  ) {}

  ngOnInit() {
    this.buildForm();
    this.setDefaultPlayerStatistics();
    this.route.params.subscribe((params: HttpParams) => {
      this.matchdayId = +params["matchId"];
      this.matchService.getOrganizedMatchInformation(this.matchdayId).subscribe(
        (res) => {
          if (res.length) {
            console.log(res);
            this.isMatchOrganized = true;
            this.mapOrganizedMatchToTeams(res);
          }
        },
        (err) => {}
      );
    });
  }
  private mapOrganizedMatchToTeams(res: any) {
    res.forEach((element) => {
      this.selectedPlayer = {
        username: element.username,
        statistics: new Statistics(
          element.goals,
          element.assists,
          element.minutesPlayed
        ),
        mainPhoto: element.image
          ? {
              image: element.image,
            }
          : null,
        firstname: element.firstname,
        lastname: element.lastname,
        created: element.created,
      };
      if (element.homeId) {
        this.addPlayerToTeam("home", false);
      }

      if (element.awayId) {
        this.addPlayerToTeam("away", false);
      }
    });
  }

  private setDefaultPlayerStatistics(): void {
    this.playerStatisticsForm.patchValue(this.defaultStatistics);
  }

  private buildForm() {
    this.playerStatisticsForm = this.fb.group({
      goals: ["", [Validators.min(0)]],
      assists: ["", [Validators.min(0)]],
      minutesPlayed: ["", [Validators.min(0)]],
    });
  }

  public selectPlayer(user: any): void {
    this.selectedPlayer = user;
    this.removePlayerToRemoveOrSwitch();
  }

  public removeSelectedPlayer(): void {
    this.selectedPlayer = null;
  }

  public removePlayerToRemoveOrSwitch(): void {
    this.playerToRemoveOrSwitch = null;
  }

  public addPlayerToTeam(team: string, addStatistics: boolean): void {
    if (!this.selectedPlayer || !team) {
      return;
    }

    if (addStatistics) {
      this.setPlayerStatistics();
    }

    if (team === "home" && this.homeTeam.players.length < 5) {
      this.homeTeam.players.push(this.selectedPlayer);
      this.removeSelectedPlayerFromList();
      this.removeSelectedPlayer();
    }
    if (team === "away" && this.awayTeam.players.length < 5) {
      this.awayTeam.players.push(this.selectedPlayer);
      this.removeSelectedPlayerFromList();
      this.removeSelectedPlayer();
    }
    this.organizedPlayersCount++;
    this.setDefaultPlayerStatistics();
  }

  private setPlayerStatistics(): void {
    if (!this.playerStatisticsForm.invalid) {
      this.selectedPlayer.statistics = new Statistics(
        +this.playerStatisticsForm.get("goals").value,
        +this.playerStatisticsForm.get("assists").value,
        +this.playerStatisticsForm.get("minutesPlayed").value
      );
    }
  }

  private removeSelectedPlayerFromList(): void {
    this.users = this.users.filter(
      (u) => u.username !== this.selectedPlayer.username
    );
  }

  public selectPlayerToRemoveOrSwitch(player: any, team: string): void {
    if (this.isMatchOrganized) {
      return;
    }

    if (!this.isSwitchActivated) {
      this.playerToRemoveOrSwitch = player;
      this.playerToRemoveOrSwitch.team = team;
      return;
    }

    var selectedPlayerIndex = this.getPlayerIndexInTeam(
      this.playerToRemoveOrSwitch.team === "home"
        ? this.homeTeam
        : this.awayTeam,
      this.playerToRemoveOrSwitch
    );
    var switchPlayerIndex = this.getPlayerIndexInTeam(
      team === "home" ? this.homeTeam : this.awayTeam,
      player
    );

    if (this.playerToRemoveOrSwitch.team === "home" && team === "home") {
      this.homeTeam.players[selectedPlayerIndex] = player;
      this.homeTeam.players[switchPlayerIndex] = this.playerToRemoveOrSwitch;
      this.removePlayerToRemoveOrSwitch();
      this.isSwitchActivated = false;
      return;
    }

    if (this.playerToRemoveOrSwitch.team === "home" && team === "away") {
      this.homeTeam.players[selectedPlayerIndex] = player;
      this.awayTeam.players[switchPlayerIndex] = this.playerToRemoveOrSwitch;
      this.removePlayerToRemoveOrSwitch();
      this.isSwitchActivated = false;
      return;
    }

    if (this.playerToRemoveOrSwitch.team === "away" && team === "away") {
      this.awayTeam.players[selectedPlayerIndex] = player;
      this.awayTeam.players[switchPlayerIndex] = this.playerToRemoveOrSwitch;
      this.removePlayerToRemoveOrSwitch();
      this.isSwitchActivated = false;
      return;
    }

    if (this.playerToRemoveOrSwitch.team === "away" && team === "home") {
      this.awayTeam.players[selectedPlayerIndex] = player;
      this.homeTeam.players[switchPlayerIndex] = this.playerToRemoveOrSwitch;
      this.removePlayerToRemoveOrSwitch();
      this.isSwitchActivated = false;
      return;
    }
  }

  private getPlayerIndexInTeam(team: Team, player: any): number {
    for (let index = 0; index < team.players.length; index++) {
      const element = team.players[index];
      if (element.username === player.username) {
        return index;
      }
    }
    return -1;
  }

  public removePlayerFromTeam(playerToRemoveOrSwitch) {
    if (playerToRemoveOrSwitch) {
      this.homeTeam.players = this.homeTeam.players.filter(
        (u) => u.username !== playerToRemoveOrSwitch.username
      );
      this.awayTeam.players = this.awayTeam.players.filter(
        (u) => u.username !== playerToRemoveOrSwitch.username
      );
      this.users.unshift(playerToRemoveOrSwitch);
    }
    this.removePlayerToRemoveOrSwitch();
    this.isSwitchActivated = false;
    this.organizedPlayersCount--;
  }

  public confirmTeams(): void {
    var matchData = {
      homeTeamName: this.homeTeam.name,
      awayTeamName: this.awayTeam.name,
      matchdayId: this.matchdayId,
      homeGoals: this.getHomeGoals(),
      awayGoals: this.getAwayGoals(),
      homeTeamMembers: this.getHomeTeamMembers(),
      awayTeamMembers: this.getAwayTeamMembers(),
    };
    this.matchService.organizeMatch(matchData).subscribe(
      (res: any) => {
        if (res.key) {
          this.notify.showSuccess("Match has been successfully organized!");
        }
      },
      (err) => {
        this.notify.showError(err);
      }
    );
  }

  private getHomeTeamMembers(): any {
    var homeTeamMembers = [];
    this.homeTeam.players.forEach((element) => {
      homeTeamMembers.push({
        userId: element.id,
        statistics: element.statistics,
      });
    });

    return homeTeamMembers;
  }

  private getAwayTeamMembers(): any {
    var awayTeamMembers = [];
    this.awayTeam.players.forEach((element) => {
      awayTeamMembers.push({
        userId: element.id,
        statistics: element.statistics,
      });
    });

    return awayTeamMembers;
  }

  public getHomeGoals(): number {
    var homeGoals = [];
    this.homeTeam.players.forEach((player) => {
      homeGoals.push(player.statistics.goals);
    });

    return homeGoals.reduce((a, b) => a + b, 0);
  }

  public getAwayGoals(): number {
    var awayGoals = [];
    this.awayTeam.players.forEach((player) => {
      awayGoals.push(player.statistics.goals);
    });

    return awayGoals.reduce((a, b) => a + b, 0);
  }
}

export enum DisplayMode {
  OrganizeMatchPlayers,
  DisplayMatchPlayers,
}
