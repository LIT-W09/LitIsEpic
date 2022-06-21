import React, { useEffect, useState, useRef } from 'react';
import { useQueryParam, NumberParam, StringParam } from 'use-query-params';
import axios from 'axios';
import { HubConnectionBuilder } from '@microsoft/signalr';

const Home = () => {
    const [id, setId] = useQueryParam('id', NumberParam);
    const [image, setImage] = useState('');
    const connectionRef = useRef(null);

    const getRandomImage = async () => {
        const { data } = await axios.get('/api/images/getrandom');
        setImage(data);
        setId(data.id);
    }

    const getById = async id => {
        const { data } = await axios.get(`/api/images/getbyid/${id}`);
        setImage(data);
        setId(data.id);
    }

    useEffect(() => {

        const connectToHub = async () => {
            const connection = new HubConnectionBuilder().withUrl("/images").build();
            await connection.start();
            connectionRef.current = connection;

            connection.on('UpdateCount', updatedImage => {
                setImage(image => {
                    if(updatedImage.id !== image.id) {
                        return image;
                    }

                    return updatedImage;
                });
            });
        }

        connectToHub();


        if (id) {
            getById(id);
        } else {
            getRandomImage();
        }
    }, []);

    if(!image){
        return <h1>Loading...</h1>
    }

    return (
        <div>
            <div className='row'>
                <div className='col text-center'>
                    <h2>Welcome to the LIT-W09 final project. Enjoy the random pictures!!</h2>
                </div>
            </div>
            <div className='row'>
                <div className='col text-center'>
                    <img style={{ width: 600 }} src={`/api/images/view/${image.id}`} />
                </div>

            </div>
            <div className='row mt-3'>
                <div className='col text-center'>
                    <button onClick={() => getRandomImage()} className='btn btn-success'>Get New Random Image</button>
                </div>
            </div>

            <div className='row mt-3'>
                <div className='col text-center'>
                    <h4>This image has been viewed {image.timesViewed} times.</h4>
                </div>
            </div>

        </div>
    )
}

export default Home;