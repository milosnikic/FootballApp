import { Group } from './group';
import { Photo } from './photo';

export class User {
    username: string;
    password?: string;
    firstname: string;
    lastname: string;
    email: string;
    dateOfBirth?: Date;
    age: number;
    lastActive?: Date;
    created: Date;
    gender: string;
    city: string;
    country: string;
    photos?: Photo[];
    groups?: Group[];
}
