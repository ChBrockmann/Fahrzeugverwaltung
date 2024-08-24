cd .\Backend

docker build -t registry.christian-brockmann.de/fahrzeugverwaltung-backend .
docker push registry.christian-brockmann.de/fahrzeugverwaltung-backend

cd ../frontend

docker build -t registry.christian-brockmann.de/fahrzeugverwaltung-frontend .
docker push registry.christian-brockmann.de/fahrzeugverwaltung-frontend