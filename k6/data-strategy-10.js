import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 20 },
    { duration: '1m30s', target: 10 },
    { duration: '20s', target: 0 },
  ],
};

export default function () {
  const res = http.get('https://localhost:5001/api/v10/Authors/1');
  check(res, { 'status was 200': (r) => r.status == 200 });
  sleep(0.1);
}
// usage
// k6 run data-strategy-1.js
