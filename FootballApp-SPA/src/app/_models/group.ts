import { MembershipStatus } from './MembershipStatus.enum';

export interface Group {
    id: number;
    name: string;
    description: string;
    numberOfMembers: number;
    dateCreated: Date;
    membershipStatus: MembershipStatus;
    favorite: boolean;
    location: any;
    image: string;
}
