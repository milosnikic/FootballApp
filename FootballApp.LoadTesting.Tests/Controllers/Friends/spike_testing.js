// Spike test is a variation of a stress test, but it does not gradually increase the load,
// instead it spikes to extreme load over a very short window of time. While a stress test 
// allows the SUT (System Under Test) to gradually scale up its infrastructure, a spike test does not.

// What is spike testing?
// Spike testing is a type of stress testing that immediately overwhelms the system with an extreme surge of load.

// You want to execute a spike test to:

// - Determine how your system will perform under a sudden surge of traffic.
// - Determine if your system will recover once the traffic has subsided.

// A classic need for a spike testing is if you've bought advertising on a big television event, 
// such as the Super Bowl or a popular singing competition.

// You’re expecting a large number of people to see your advertisement and immediately visit your website, 
// which can end with disastrous results if you haven't tested for this scenario and made performance optimizations in advance.

// Another typical example is a "HackerNews hug of death" - someone links to your website on one of the popular internet forums 
// such as HackerNews or Reddit which makes thousands of people visit your system at the same time.

// Success or failure of a spike test depends on your expectations. Systems generally react in 4 different ways:

// Excellent: system performance is not degraded during the surge of traffic. Response time is similar during low traffic and high 
//            traffic.
// Good: Response time is slower, but the system does not produce any errors. All requests are handled.
// Poor: System produces errors during the surge of traffic, but recovers to normal after the traffic subsides.
// Bad: System crashes, and does not recover after the traffic has subsided.

import http from 'k6/http';
import { sleep } from 'k6';

// Note, the test starts with a period of 1 minute of low load, a quick spike to very high load, followed by a 
// recovery period of low load.

// Remember that the point of this test is to suddenly overwhelm the system. Don't be afraid to increase the 
// number of VUs beyond your worst-case prediction. Depending on your needs, you may want to extend the recovery 
// stage to 10+ minutes to see when the system finally recovers.

export let options = {
  stages: [
    { duration: '10s', target: 100 }, // below normal load
    { duration: '1m', target: 100 },
    { duration: '10s', target: 1400 }, // spike to 1400 users
    { duration: '3m', target: 1400 }, // stay at 1400 for 3 minutes
    { duration: '10s', target: 100 }, // scale down. Recovery stage.
    { duration: '3m', target: 100 },
    { duration: '10s', target: 0 },
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

export default function () {

  let responses = http.batch([
    [
      'GET',
      `${BASE_URL}/public/crocodiles/1/`,
      null,
      { tags: { name: 'PublicCrocs' } },
    ],
    [
      'GET',
      `${BASE_URL}/public/crocodiles/2/`,
      null,
      { tags: { name: 'PublicCrocs' } },
    ],
    [
      'GET',
      `${BASE_URL}/public/crocodiles/3/`,
      null,
      { tags: { name: 'PublicCrocs' } },
    ],
    [
      'GET',
      `${BASE_URL}/public/crocodiles/4/`,
      null,
      { tags: { name: 'PublicCrocs' } },
    ],
  ]);

  sleep(1);
}
