import { Component, OnInit, Input } from "@angular/core";
import { User } from "src/app/_models/user";
import { MembershipStatus } from "src/app/_models/MembershipStatus.enum";
import { Role } from "src/app/_models/role.enum";
import { MatchService } from "src/app/_services/match.service";

@Component({
  selector: "app-group-members",
  templateUrl: "./group-members.component.html",
  styleUrls: ["./group-members.component.css"],
})
export class GroupMembersComponent implements OnInit {
  @Input() groupMembers: User[];
  // Membership info has 3 fields
  //            favorite
  //            status
  //            role
  @Input() membershipInfo: any;
  Role = Role;
  selectedUser: any = null;
  userForm: any = [];

  constructor(private matchService: MatchService) {}

  ngOnInit() {}

  setSelectedUser(user: any) {
    this.selectedUser = user;
    this.userForm = [];
    this.matchService.getMatchHistoryForUser(user.id).subscribe(
      (res) => {
        res = res.slice(Math.max(res.length - 5, 0));
        res.forEach((element) => {
          this.userForm.push(element.result.charAt(0));
        });
      },
      (err) => {}
    );
  }

  getFormColor(match): { color: string } {
    switch (match) {
      case "W":
        return { color: "greenyellow" };
      case "D":
        return { color: "yellow" };
      case "L":
        return { color: "red" };
    }
  }
}
