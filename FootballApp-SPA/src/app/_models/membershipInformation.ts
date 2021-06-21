import { MembershipStatus } from "./MembershipStatus.enum"
import { Role } from "./role.enum"

export class MembershipInformation {
    favorite: boolean;
    membershipStatus: MembershipStatus;
    role: Role;
}
