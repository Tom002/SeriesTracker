import { ISeriesWatchedListItem } from "../../app/models/series";
import { Card, Image } from "semantic-ui-react";
import React from 'react';
import { Link } from "react-router-dom";


const SeriesLikedCard: React.FC<{ series: ISeriesWatchedListItem }> = ({
    series
}) => {
    return (
        <Card style={{marginTop: '2em' }}>
            <Image src={series.coverImageUrl} as={Link} to={`/series/${series.seriesId}`} />
            <Card.Content>
                <Card.Header>{series.title}</Card.Header>
                <Card.Meta>{series.startYear ? series.startYear+ "-" +(series.endYear || 'now') : ""}</Card.Meta>
            </Card.Content>
        </Card>
    )
}
export default SeriesLikedCard;