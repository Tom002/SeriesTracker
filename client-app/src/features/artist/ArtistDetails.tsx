import { observer } from "mobx-react-lite";
import React, { Fragment } from "react";
import { useContext, useEffect } from "react";
import { RouteComponentProps } from "react-router-dom";
import { Container, Grid, GridColumn, Header, Label, LabelGroup, Image } from "semantic-ui-react";
import RootStore from "../../app/stores/rootStore";
import AppearedInSeriesCard from "./AppearedInSeriesCard";
import WriterOfSeriesCard from "./WriterOfSeriesCard";

interface ArtistDetailParams {
    id: string;
  }

const ArtistDetails: React.FC<RouteComponentProps<ArtistDetailParams>> = ({
    match,
    history,
  }) => {
    const rootStore = useContext(RootStore);
    const {
        loadSingleArtist,
        clearSelectedArtist,
        selectedArtist
    } = rootStore.artistStore

    useEffect(() => {
        console.log(match.params.id);
        if (match.params.id) {
            let id = Number(match.params.id);
            if(!isNaN(id)) {
                loadSingleArtist(id);
            }
        }
        return () => {
            clearSelectedArtist();
        }  
    }, [])

    return (
        <Fragment>
            <Grid  centered columns={2} style={{ marginLeft: "2em", marginTop: "2em", marginRight: "2em" }}>
                <Grid.Column width={6} color='black' inverted>
                    <Image src={selectedArtist?.imageUrl} size="medium" />
                </Grid.Column>
                <Grid.Column width={6} textAlign="left" color='black' inverted>
                    <Header textAlign="center" size="huge" color='black' inverted>
                        {selectedArtist?.name}
                    </Header>
                    <Container>{selectedArtist?.about}</Container>
                </Grid.Column>
            </Grid>

            {selectedArtist?.writerOf && selectedArtist.writerOf.length > 0 && (
                <Fragment>
                    <Header textAlign="center">
                        Writer of:
                    </Header>

                    <Grid columns={5} container doubling stackable>
                    {Array.from(selectedArtist.writerOf).map(writerOf => (
                        <Grid.Column>
                            <WriterOfSeriesCard writerOf={writerOf} />
                        </Grid.Column>
                    ))}
                    </Grid>
                </Fragment>
            )}

            {selectedArtist?.appearedIn && selectedArtist.appearedIn.length > 0 && (
                <Fragment>
                    <Header size="huge" textAlign="center"> 
                        Appeared In:
                    </Header>

                    <Grid columns={5} container doubling stackable>
                    {Array.from(selectedArtist.appearedIn).map(appearance => (
                        <Grid.Column>
                            <AppearedInSeriesCard appearedIn={appearance} />
                        </Grid.Column>
                    ))}
                    </Grid>
                </Fragment>
            )}
        </Fragment>
    )
}

export default observer(ArtistDetails);