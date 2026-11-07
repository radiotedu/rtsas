# Listener isolation candidate

This branch is an experimental source change. It is not installed on the live server and is not a release.

The candidate separates each listener's writes behind a bounded queue, so a slow listener cannot block the source or healthy listeners, and it does not treat a matching IP address and user agent as proof that an existing listener is stale. Ogg clients receive the captured header only once before the cached page-aligned stream. The tests use synthetic bytes over literal loopback sockets; they do not establish real-device, public-network, or long-duration codec compatibility.

A source disconnection still closes listeners. Concurrent source connections are not fenced: an old source can close listeners after a new source connects. Server or proxy restarts can interrupt all listeners. These limits are not fixed by this candidate. Production rollout requires separate server administrator access, deployment/rollback planning, real AAC and Ogg FLAC listener tests, and long-duration observation.
