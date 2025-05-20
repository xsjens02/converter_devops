// This is a spike test designed to simulate a sudden and extreme increase in load
// on the API to evaluate its performance under unexpected traffic spikes.

// The purpose of this test is to identify how the system handles rapid spikes in traffic and whether it can
// recover without significant performance degradation. The system's behavior during the spike, such as response
// times and failure rates, will be monitored to determine how well it can manage short-term bursts in load.
import http from 'k6/http';
import { sleep, check } from 'k6';

const baseUrl = 'http://79.76.48.213:5000';

export const options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '10s', target: 50 },  // normal load with 50 users (50 x 12 => 600requests)
        { duration: '30s', target: 50 },  // stay at 50 users for 30 seconds
        { duration: '10s', target: 200 },  // spike up to 200 users (200 x 12 => 2400requests)
        { duration: '1m', target: 200 },   // stay at 200 users for 1 minute
        { duration: '10s', target: 50 },   // scale down to normal load with 50 users (50 x 12 => 600requests)
        { duration: '30s', target: 50 },   // stay at 50 users for 30 seconds
        { duration: '10s', target: 0 },    // fade out
    ],
};

export default () => {

    let responses = http.batch([
        ['GET', `${baseUrl}/api/volumes/convert?value=10&from=2&to=3`],
        ['GET', `${baseUrl}/api/volumes/add?a=10&aUnit=2&b=500&bUnit=0&resultUnit=3`],
        ['GET', `${baseUrl}/api/volumes/subtract?a=8&aUnit=2&b=500&bUnit=0&resultUnit=3`],
        ['GET', `${baseUrl}/api/volumes/difference?a=5&aUnit=2&b=3&bUnit=3&resultUnit=3`],
        ['GET', `${baseUrl}/api/volumes/scale?value=2&valueUnit=2&factor=5&resultUnit=3`],
        ['GET', `${baseUrl}/api/volumes/percentage?a=1&part=2&b=1&whole=3`],
        ['GET', `${baseUrl}/api/weights/convert?value=10&from=2&to=3`],
        ['GET', `${baseUrl}/api/weights/add?a=10&aUnit=2&b=500&bUnit=0&resultUnit=3`],
        ['GET', `${baseUrl}/api/weights/subtract?a=8&aUnit=2&b=500&bUnit=0&resultUnit=3`],
        ['GET', `${baseUrl}/api/weights/difference?a=5&aUnit=2&b=3&bUnit=3&resultUnit=3`],
        ['GET', `${baseUrl}/api/weights/scale?value=2&valueUnit=2&factor=5&resultUnit=3`],
        ['GET', `${baseUrl}/api/weights/percentage?a=1&part=2&b=1&whole=3`]
    ]);

    sleep(1);
};