// Load Testing is primarily concerned with assessing the current performance of your system in terms of concurrent 
// users or requests per second.

// When you want to understand if your system is meeting the performance goals, this is the type of test you'll run.

// What is Load Testing
// Load Testing is a type of Performance Testing used to determine a system's behavior under both normal and peak conditions.

// Load Testing is used to ensure that the application performs satisfactorily when many users access it at the same time.

// You should run Load Test to:

// - Assess the current performance of your system under typical and peak load.
// - Make sure you are continuously meeting the performance standards as you make changes to your system (code and infrastructure).

// You probably have some understanding about the amount of traffic your system is seeing on average and during peak hours.
// This information will be useful when deciding what your performance goals should be, in other words, how to configure 
// the performance thresholds.

// Let's say you're seeing around 60 concurrent users on average and 100 users during the peak hours of operation.

// It's probably important to you to meet the performance goals both during normal hours and peak hours, therefore 
// it's recommended to configure the load test with the high load in mind - 100 users in this case.

import http from 'k6/http';
import { check, group, sleep } from 'k6';

// Note, this test has one simple threshold. The response time for 99% requests must be below 1.5 seconds. 
// Thresholds are a way of ensuring that your system is meeting the performance goals you set for it.


// Image: https://k6.io/docs/static/53c756573c738528633ed7b67a7819df/52df6/load-test.png
// Note that the number of users starts at 0, and slowly ramps up to the nominal value, 
// where it stays for an extended period of time. The ramp down stage is optional.

// It is recommended that you always include a ramp-up stage in all your Load Tests because:
// - it allows your system to warm up or auto scale to handle the traffic
// - it allows you to compare the response time between the low-load and nominal-load stages.
// - If you run a load test using the SaaS cloud service, it allows the automated performance 
//   alerts to better understand the normal behaviour of your system.
export let options = {
  stages: [
    { duration: '5m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
    { duration: '10m', target: 100 }, // stay at 100 users for 10 minutes
    { duration: '5m', target: 0 }, // ramp-down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
    'logged in successfully': ['p(99)<1500'], // 99% of requests must complete below 1.5s
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

  sleep(1);
};
