export class Statistics {
  goals: number;
  assists: number;
  minutesPlayed: number;
  rating: number;

  constructor(goals: number, assists: number, minutesPlayed: number) {
    this.goals = goals;
    this.assists = assists;
    this.minutesPlayed = minutesPlayed;
  }
}
