import React from "react";
import { Link } from "react-router-dom";
import { Card, Image, Label } from "semantic-ui-react";
import { IActorInSeries } from "../../app/models/artist";

const AppearedInSeriesCard: React.FC<{appearedIn: IActorInSeries}> = ({
    appearedIn
}) => {

    return (
        <Card>
            <Image src={appearedIn.coverImageUrl} as={Link} to={`/series/${appearedIn.seriesId}`} />
            <Card.Content>
                <Card.Header>{appearedIn.title}</Card.Header>
                <Label.Group color='blue'>
                <Label as = 'a'>
                    as {appearedIn.roleName}
                </Label>
                </Label.Group>
            </Card.Content>
        </Card>
    )
}

export default AppearedInSeriesCard;