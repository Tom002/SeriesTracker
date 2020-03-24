# Sorozatkövető alkalmazás, microservices architektúrával

Az alkalmazás a következő microservicekből épül fel:
* `BrowsingService`: Bejelentkezés nélkül elérhető katalógus a sorozatok és színészek közötti keresésre, szűrésre
* `ReviewService`: Bejelentkezés után elérhető, szöveges és számszerű értékelések írása sorozatokhoz és epizódokhoz
* `WatchingService`: Bejelentkezés után elérhető, felhasználók megjelölhetik milyen sorozatokat néznek, kedvelnek és hogy milyen részeket láttak már az adott sorozatból
* `ProfileService`: Bejelentkezés után elérhető, saját és más felhasználók profiljának megtekintése.
* `IdentityService`: Bejelentkezés és regisztráció funkció, IdentityServer 4-el.
