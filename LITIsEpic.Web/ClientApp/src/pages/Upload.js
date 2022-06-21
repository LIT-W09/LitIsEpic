import React, { useRef } from "react";
import axios from 'axios';
import { useHistory } from 'react-router-dom';

const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});

const Upload = () => {
    const fileInputRef = useRef(null);
    const history = useHistory();
    const onUploadClick = async () => {
        const file = fileInputRef.current.files[0];
        const parts = file.name.split('.');
        const fileExtension = parts[parts.length - 1];
        const base64File = await toBase64(file);
        const { data } = await axios.post('/api/images/upload', { base64File, fileExtension });
        history.push(`/?id=${data.id}`);
    }

    return (
        <div className="d-flex vh-100" style={{ marginTop: -70 }}>
            <div className="d-flex w-100 justify-content-center align-self-center">
                <div className="row">
                    <h3 style={{marginLeft:30, width:'100%'}}>Upload an image</h3>
                    <div className="col-md-8">
                        <input ref={fileInputRef} type="file" className='form-control-lg' />
                    </div>
                    <div className='col-md-4'>
                        <button className='btn btn-primary btn-block' onClick={onUploadClick}>Upload</button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Upload;