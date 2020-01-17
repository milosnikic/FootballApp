import { GroupStatus } from './GroupStatus.enum';
export interface Group {
    name: string;
    description: string;
    numberOfMembers: number;
    dateCreated: Date;
    member: boolean;
}
