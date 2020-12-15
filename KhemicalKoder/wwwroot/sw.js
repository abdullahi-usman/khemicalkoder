
const cacheName = 'khemicalkoder-v1';

const cacheContents = [
    '/favicon.ico',
    '/lib/bootstrap/dist/css/bootstrap.min.css',
    '/lib/jquery/dist/jquery.min.js',
    '/lib/bootstrap/dist/js/bootstrap.bundle.min.js'
]

self.addEventListener('install', event => {
    event.waitUntil(caches.open(cacheName).then(cache => {
        return cache.addAll(cacheContents);
    }));
})

self.addEventListener('activate', event => {
    
});

self.addEventListener('fetch', event => {
    event.respondWith(caches.match(event.request).then(res => {
        return res || fetch(event.request);
    }))
});