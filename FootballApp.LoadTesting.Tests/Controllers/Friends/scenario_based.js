import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { Trend } from 'k6/metrics';

let LoginTrend = new Trend('Login');
let GetUsersTrend = new Trend('Get Users');
let GetGroupsTrend = new Trend('Get Groups');

export let options = {
  //   vus: 1000,
  //   duration: '600s',
  vus: 100,
  duration: '10s',
  thresholds: {
    login: ['p(95)<500'],
    'get-users': ['p(95)<800'],
  },
};

const SLEEP_DURATION = 0.1;
const BASE_URL = 'http://localhost:5000/api';

export default function () {
  let loginUrl = `${BASE_URL}/Auth/login`;
  let usersUrl = `${BASE_URL}/Users`;
  let groupsUrl = `${BASE_URL}/Groups`;

  let body = JSON.stringify({
    username: 'milos',
    password: 'Test123*',
  });

  let params = {
    headers: {
      'Content-Type': 'application/json',
    },
    tags: {
      name: 'login', // first request
    },
  };

  group('Common user scenario:', (_) => {
    // Login request
    let login_response = http.post(loginUrl, body, params);
    check(login_response, {
      'is login status 200': (r) => r.status === 200,
      'is token present': (r) => r.json().hasOwnProperty('token'),
    });

    let userId = login_response.json()['user'].id;

    params.headers['Authorization'] = `Bearer ${
      login_response.json()['token']
    }`;
    LoginTrend.add(login_response.timings.duration);
    sleep(SLEEP_DURATION);

    // Get users
    params.tags.name = 'get-users';
    let users_response = http.get(usersUrl, params);
    check(users_response, {
      'is get-users status 200': (r) => r.status === 200,
    });
    GetUsersTrend.add(users_response.timings.duration);
    sleep(SLEEP_DURATION);

    // Get groups
    params.tags.name = 'get-groups';
    let groups_response = http.get(groupsUrl + `?userId=${userId}`, params);
    check(groups_response, {
      'is get-groups status 200': (r) => r.status === 200,
    });
    GetGroupsTrend.add(groups_response.timings.duration);
    sleep(SLEEP_DURATION);

  });
}
