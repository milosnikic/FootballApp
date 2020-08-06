import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-group-members',
  templateUrl: './group-members.component.html',
  styleUrls: ['./group-members.component.css']
})
export class GroupMembersComponent implements OnInit {
  selectedUser: any = null;
  users: any = [
    {
      firstname: 'Milos',
      lastname: 'Nikic',
      username: 'kicni',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'milos.nikic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth:'31.12.1996.'
    },
    {
      firstname: 'jovan',
      lastname: 'jovanovic',
      username: 'joca',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'jovan.jovanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth:'31.12.1996.'
    },
    {
      firstname: 'Ivan',
      lastname: 'Ivanovic',
      username: 'ica',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'ivan.ivanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth:'31.12.1996.'
    },
    {
      firstname: 'Ivan',
      lastname: 'Ivanovic',
      username: 'ica',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'ivan.ivanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth:'31.12.1996.'
    },
    {
      firstname: 'Ivan',
      lastname: 'Ivanovic',
      username: 'ica',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'ivan.ivanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth:'31.12.1996.'
    }
  ];

  constructor() { }

  ngOnInit() {
  }

  setSelectedUser(user: any) {
    this.selectedUser = user;
  }
}
