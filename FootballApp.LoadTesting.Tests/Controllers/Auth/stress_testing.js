/*
Stress testing is one of many different types of load testing.

While load testing is primarily concerned with assessing the systems performance, the purpose of stress testing is to assess the availability and stability of the system under heavy load.

What is stress testing?
Stress Testing is a type of load testing used to determine the limits of the system. The purpose of this test is to verify the stability and reliability of the system under extreme conditions.

You typically want to stress test an API or website to:

- determine how your system will behave under extreme conditions.
- determine what is the maximum capacity of your system in terms of users or throughput.
- determine the breaking point of your system and its failure mode.
- determine if your system will recover without manual intervention after the stress test is over.

When stress testing, you're going to configure the test to include more concurrent users or generate higher throughput than:

- your application typically sees.
- you think it will be able to handle.

It's important to note that a stress test does not mean you're going to overwhelm the system immediately — that's a spike test.

A stress test should be configured in many gradual steps, each step increasing the concurrent load of the system.

    See example in image: https://k6.io/docs/static/5a1571e3a4df83a907e0346e586c784f/e134c/stress-test.png

A classic example of a need for stress testing is "Black Friday" or "Cyber Monday" - two days each year that generates multiple times the normal traffic for many websites.

A stress test can be only a couple of steps, or it can be many, as you see in the example below. No matter how many steps you include, just remember this type of test is about finding out what happens when pushing the performance limits of your system — so don’t worry about being too aggressive.
*/

import http from 'k6/http';
import { sleep, check } from 'k6';

// This configuration increases the load by 100 users every 2 minutes and stays 
// at this level for 5 minutes. We have also included a recovery stage at the end, 
// where the system is gradually decreasing the load to 0.


// If your infrastructure is configured to auto-scale, this test will help you to determine:

// - How quickly the auto-scaling mechanisms react to increased load.
// - Are there any failures during the scaling events.

// The point of the recovery stage is to determine if the system can serve requests once the load 
// decreases to a normal level. If you are testing auto-scaling, you may want to scale down in steps
// as well to determine if the down-scaling is working.
export let options = {
  stages: [
    { duration: '2m', target: 100 }, // below normal load
    { duration: '5m', target: 100 },
    { duration: '2m', target: 200 }, // normal load
    { duration: '5m', target: 200 },
    { duration: '2m', target: 300 }, // around the breaking point
    { duration: '5m', target: 300 },
    { duration: '2m', target: 400 }, // beyond the breaking point
    { duration: '5m', target: 400 },
    { duration: '10m', target: 0 }, // scale down. Recovery stage.
  ],
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
