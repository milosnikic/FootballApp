import { User } from "./user";

export class Team {
  name: string;
  players: User[] = [];

  constructor(name: string) {
    this.name = name;
  }
}
