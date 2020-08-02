import { GroupStatus } from './GroupStatus.enum';
export interface Group {
    id: number;
    name: string;
    description: string;
    numberOfMembers: number;
    dateCreated: Date;
    membershipStatus: GroupStatus;
    favorite: boolean;
    location: any;
    image: string;
}
