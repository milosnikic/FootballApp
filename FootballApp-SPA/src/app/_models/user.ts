import { Group } from './group';

export interface User {
    username: string;
    password: string;
    firstname: string;
    lastname: string;
    email: string;
    dateOfBirth: Date;
    lastActive: Date;
    created: Date;
    gender: string;
    city: string;
    country: string;
    groups: Group[];
}
