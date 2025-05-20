// This is a load test designed to evaluate how the API handles
// a sustained load over a longer period of time.
// With 50 users running in parallel, this generates a consistent and considerable load on the server.

// The main purpose of this test is to observe the system's performance under stable and
// expected conditions — such as memory usage, response time, and request throughput —
// and to ensure that 95% of requests complete in under 150ms, as defined in the thresholds.

// This test helps validate that the system can handle normal peak traffic without degrading
// or failing, and it provides insight into its reliability under steady pressure.
import http from 'k6/http';
import { sleep, check } from 'k6';

const baseUrl = 'http://79.76.48.213:5000';

export const options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '1m', target: 50 },  // ramp-up from 0 to 50 users over 1 minutes (50 x 12 => 600requests)
        { duration: '3m', target: 50 },  // stay at 50 users for 3 minutes 
        { duration: '1m', target: 0 },    // ramp-down to 0 users over 1 minutes
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