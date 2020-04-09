import { ISeries } from "../../app/models/series";
import { Card, Image, Label } from "semantic-ui-react";
import React from 'react';
import { Link } from "react-router-dom";


const SeriesCard: React.FC<{ series: ISeries }> = ({
    series
}) => {
    return (
        <Card>
            <Image src={series.coverImageUrl} as={Link} to={`/series/${series.seriesId}`} />
            <Card.Content>
                <Card.Header>{series.title}</Card.Header>
                <Card.Meta>{series.startYear ? series.startYear+ "-" +(series.endYear || 'now') : ""}</Card.Meta>
            </Card.Content>
            <Card.Content extra>
                <Label.Group color='blue'>
                        {
                            series.categories.map(category => (
                                <Label as='a'>
                                    {category.categoryName}
                                </Label>
                            ))
                        }
                </Label.Group>
            </Card.Content>
        </Card>
    )
}
export default SeriesCard;