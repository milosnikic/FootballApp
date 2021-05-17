import { Component, OnInit, EventEmitter } from "@angular/core";
import { MatchStatus } from "src/app/_models/matchStatus.enum";
import { MatchService } from "src/app/_services/match.service";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { NotifyService } from "src/app/_services/notify.service";
import { first } from "rxjs/operators";
import { GroupsService } from "src/app/_services/groups.service";
import { MembershipStatus } from "src/app/_models/MembershipStatus.enum";
import { MembershipInformation } from "src/app/_models/membershipInformation";
import { Role } from "src/app/_models/role.enum";

@Component({
  selector: "app-upcoming-match",
  templateUrl: "./upcoming-match.component.html",
  styleUrls: ["./upcoming-match.component.css"],
})
export class UpcomingMatchComponent implements OnInit {
  isChecked;
  isConfirmed;
  user: any;

  groupId: number;
  users: any = [];
  match: any;
  matchId: number;
  upcomingMatches = new EventEmitter<number>();
  membershipInfo: MembershipInformation;
  MembershipStatus = MembershipStatus;
  Role = Role;
  selectedSection: UpcomingMatchSection;
  UpcomingMachSection = UpcomingMatchSection;

  constructor(
    private matchService: MatchService,
    private route: ActivatedRoute,
    private notifyService: NotifyService,
    private router: Router,
    private groupService: GroupsService
  ) {}

  ngOnInit() {
    this.selectedSection = UpcomingMatchSection.AppliedUsers;
    this.user = JSON.parse(localStorage.getItem("user"));
    this.route.paramMap.subscribe((data: ParamMap) => {
      this.matchId = +data.get("matchId");
      this.matchService
        .getUpcomingMatchInfo(this.matchId)
        .subscribe((res: any) => {
          this.match = res;
          this.groupId = this.match.groupId;
          if (this.groupId) {
            this.groupService
              .getMembershipInformation(this.groupId, this.user.id)
              .subscribe((res: any) => {
                this.membershipInfo = res;
              });
          }
          this.users = this.match.appliedUsers;
        });
      this.matchService
        .getUserMatchStatus(this.matchId, this.user.id)
        .subscribe((res: any) => {
          // Here we have match status {checked: bool, confirmed: bool}
          this.isChecked = res !== null ? res.checked : false;
          this.isConfirmed = res !== null ? res.confirmed : false;
        });
    });
  }

  checkIn() {
    this.matchService.checkIn(this.user.id, this.matchId).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess(res.value);
          this.isChecked = true;
          this.user.matchStatus = MatchStatus.Checked;
          this.users.unshift(this.user);
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        this.notifyService.showError(err.error);
      }
    );
  }

  confirm() {
    this.matchService.confirm(this.user.id, this.matchId).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess(
            "Successfully confirmed participation!"
          );
          this.isChecked = false;
          this.isConfirmed = true;
          this.users.find(
            (u) => u.firstname === this.user.firstname
          ).matchStatus = MatchStatus.Confirmed;
          this.match.numberOfConfirmedPlayers++;
        } else {
          this.notifyService.showError("Problem confirming participation!");
        }
      },
      (err) => {
        this.notifyService.showError(err.error);
      }
    );
  }

  giveUp() {
    this.isChecked = false;
    this.matchService.giveUp(this.user.id, this.matchId).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess("Successfully gave up match!");
          this.users = this.users.filter(
            (u) => u.firstname !== this.user.firstname
          );
        } else {
          this.notifyService.showError("Problem giving up a match!");
        }
      },
      (err) => {
        this.notifyService.showError(err.error);
      }
    );
  }

  getCapacityClass() {
    if (this.users.length === 0) {
      return "zero-capacity";
    } else if (this.users.length < 5) {
      return "low-capacity";
    } else if (this.users.length < 8) {
      return "moderate-capacity";
    } else if (this.users.length < 11) {
      return "high-capacity";
    }
    return "full-capacity";
  }

  goBack() {
    this.router.navigate(["../.."], { relativeTo: this.route });
  }

  public organizeMatch(): void {
    this.selectedSection = UpcomingMatchSection.OrganizeUserByTeam;
  }
}

export enum UpcomingMatchSection {
  AppliedUsers,
  OrganizeUserByTeam,
}
