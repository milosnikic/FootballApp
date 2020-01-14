import { Group } from './group';
import { Photo } from './photo';

export class User {
    username: string;
    firstname: string;
    password?: string;
    lastname: string;
    email: string;
    dateOfBirth: Date;
    lastActive: Date;
    created: Date;
    gender: string;
    city: string;
    country: string;
    photos?: Photo[];
    groups?: Group[];
}
