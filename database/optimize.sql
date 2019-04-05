USE f1db;

CREATE INDEX driverLap ON lapTimes(driverId);

CREATE INDEX pitStopRace on pitStops(raceId);
CREATE INDEX pitStopDriver on pitStops(driverId);

CREATE INDEX qualifyingDriver on qualifying(driverId);
CREATE INDEX qualifyingRace on qualifying(raceId);

CREATE INDEX resultsRace on results(raceId);
CREATE INDEX resultsDriver on results(driverId);
CREATE INDEX resultsConstructor on results(constructorId);
