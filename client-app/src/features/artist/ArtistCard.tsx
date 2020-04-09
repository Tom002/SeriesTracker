import { IArtist } from "../../app/models/artist";
import { Card, Image, Label } from "semantic-ui-react";
import { Link } from "react-router-dom";
import React from "react";
import { ageFromDateOfBirth } from "../../app/helpers/util";


const ArtistCard: React.FC<{artist: IArtist}> = ({
    artist
}) => {
    return (
        <Card>
            <Image src={artist.imageUrl} as={Link} to={`/artists/${artist.artistId}`} />
            <Card.Content>
                <Card.Header>{artist.name}</Card.Header>
                {artist.birthDate && artist.deathDate == null && (
                      <Card.Meta>
                          {ageFromDateOfBirth(artist.birthDate)} years old
                      </Card.Meta>) }
            </Card.Content>
            <Card.Content extra>
                <Label.Group color='blue'>
                        { artist.appearedInCount > 0 && (
                                <Label as = 'a'>
                                    Actor in {artist.appearedInCount} Series
                                </Label>) }
                        { artist.writerOfCount > 0 && (
                                <Label as = 'a'>
                                    Writer of {artist.writerOfCount} Series
                                </Label>) }
                </Label.Group>
            </Card.Content>
        </Card>
    )
}

export default ArtistCard;