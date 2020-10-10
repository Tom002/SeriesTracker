import React from "react";
import { Link } from "react-router-dom";
import { Card, Image } from "semantic-ui-react";
import { IWriterOfSeries } from "../../app/models/artist";

const WriterOfSeriesCard: React.FC<{writerOf: IWriterOfSeries}> = ({
    writerOf
}) => {

    return (
        <Card>
            <Image src={writerOf.coverImageUrl} as={Link} to={`/series/${writerOf.seriesId}`} />
            <Card.Content>
                <Card.Header>{writerOf.title}</Card.Header>
            </Card.Content>
        </Card>
    )
}

export default WriterOfSeriesCard;