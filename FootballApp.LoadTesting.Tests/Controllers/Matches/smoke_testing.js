// Smoke test is a regular load test, configured for minimal load. You want to run a smoke
// test as a sanity check every time you write a new script or modify an existing script.

// You want to run a smoke test to:

// - Verify that your test script doesn't have errors.
// - Verify that your system doesn't throw any errors when under minimal load.

// Image: https://k6.io/docs/static/243effef66c366044cc692f439cfb9a3/448f2/smoke-test.png

import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';

export let options = {
  vus: 1, // 1 user looping for 1 minute
  duration: '1m',

  thresholds: {
    http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
  },
};

const BASE_URL = 'http://localhost:5000/api';
const USERNAME = 'milos';
const PASSWORD = 'Test123*';
const body = JSON.stringify({
  USERNAME,
  PASSWORD,
});

let params = {
  headers: {
    'Content-Type': 'application/json',
  },
  tags: {
    name: 'login', // first request
  },
};

export default () => {
  // Login request
  let login_response = http.post(`${BASE_URL}/Auth/login`, body, params);
  check(login_response, {
    'is login status 200': (r) => r.status === 200,
    'is token present': (r) => r.json().hasOwnProperty('token'),
  });

  let userId = login_response.json()['user'].id;

  params.headers['Authorization'] = `Bearer ${login_response.json('token')}`;

  // Getting user friends
  let userFriendsResponse = http.get(`${BASE_URL}/Friends/${userId}`, params);
  check(userFriendsResponse, {
    'retrieved friends': (r) => r.status === 200 && r.json().length >= 0,
  });

  sleep(1);
};
