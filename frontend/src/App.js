import React, {useState, useEffect} from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
import { Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';


function App() {
  const baseUrl="https://localhost:7062/api/students"
  const [data, setData] = useState([]);
  const [modalInclude, setModalInclude]=useState(false); 
  const [modalEdit, setModalEdit]=useState(false);
  const [modalDelete, setModalDelete]=useState(false);
  const [updateData, setUpdateData] = useState(true);

  const [studentSelected, setStudentselected] = useState({
    id: '',
    name: '',
    email: '',
    age: ''
  })

  const openCloseModalInclude=()=>{
    setModalInclude(!modalInclude);
  }

  const openCloseModalEdit=()=> {
    setModalEdit(!modalEdit)
  }

  const openCloseModalDelete=()=>{
    setModalDelete(!modalDelete)
  }

  const handleChange = e=>{
    const{name,value} = e.target;
    setStudentselected({
      ...studentSelected,[name]:value
    });
    console.log(studentSelected)
  }

  const selectStudent=(student, option)=>{
    setStudentselected(student);
        (option === "Edit") ? openCloseModalEdit() : openCloseModalDelete();
  }

  const requestGet = async()=>{
      await axios.get(baseUrl)
      .then(response => 
        {
          setData(response.data);
        }).catch(error=>{
          console.log(error);
        })
  }

  const requestPost = async()=>{
    delete studentSelected.id;
    studentSelected.age=parseInt(studentSelected.age);
    await axios.post(baseUrl, studentSelected)
    .then(response=>{
      setData(data.concat(response.data));
      openCloseModalInclude();
    }).catch(error=>{
      console.log(error);
    })
  }

  const requestPut=async()=>{
    studentSelected.age=parseInt(studentSelected.age);
    await axios.put(baseUrl+"/"+studentSelected.id, studentSelected)
    .then(response=>{
      var responseD = response.data
      var auxDate = data;
      auxDate.map(student=>{
        if(student.id===studentSelected.id){
          student.name = response.name;
          student.email = response.email;
          student.email = response.email;
        }
      });
      openCloseModalEdit();
    }).catch(error=>{
      console.log(error);
    })
  }

  const requestDelete=async()=>{
    await axios.delete(baseUrl+"/"+studentSelected.id)
    .then(response=>{
      setData(data.filter(student => student.id !== response.data));
      openCloseModalDelete();
    }).catch(error=>{
      console.log(error)
    })
  }


  useEffect(()=>{
    if(updateData){
      requestGet();
      setUpdateData(false);
    }
  },[updateData])

    return (
      <div className="App">
        <br/>
        <h3>Students Register</h3>
        <header>
          <button className='btn btn-success' onClick={() => openCloseModalInclude()}>Add new student</button>
        </header>
        <br/>
        <table className='table table-bordered'>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Email</th>
              <th>Age</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {data.map(student=>(
              <tr key={student.id}>
                <td>{student.id}</td>
                <td>{student.name}</td>
                <td>{student.email}</td>
                <td>{student.age}</td>
                <td>
                  <buton className="btn btn-primary" onClick={()=>selectStudent(student, "Edit")}>Edit</buton>{" "}
                  <buton className="btn btn-danger" onClick={()=>selectStudent(student, "Delete")}>Delete</buton>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        <Modal isOpen={modalInclude}>
        <ModalHeader>Add Student</ModalHeader>
        <ModalBody>
          <div className='form-group'>
            <label>Name:</label>
            <br/>
            <input type='text' className='form-control' name='name' onChange={handleChange}/>
            <br/>
            <label>Email:</label>
            <br/>
            <input type='text' className='form-control' name='email' onChange={handleChange}/>
            <br/>
            <label>Age:</label>
            <br/>
            <input type='text' className='form-control' name='age' onChange={handleChange}/>
            <br/>
          </div>
        </ModalBody>
        <ModalFooter>
          <button className='btn btn-primary' onClick={() => requestPost()}>Add</button>{"   "}
          <button className='btn btn-danger' onClick={() => openCloseModalInclude()}>Cancel</button>
        </ModalFooter>
        </Modal>

        <Modal isOpen={modalEdit}>
        <ModalHeader>Edit Student</ModalHeader>
        <ModalBody>
          <div className='form-group'>
            <label>ID: </label><br/>
            <input type="text" className='form-control' readOnly value={studentSelected && studentSelected.id}/><br/>
            <br/>
            <label>Name:</label>
            <br/>
            <input type='text' className='form-control' name='name' onChange={handleChange}
                   value={studentSelected && studentSelected.name}/>
            <br/>
            <label>Email:</label>
            <br/>
            <input type='text' className='form-control' name='email' onChange={handleChange}
                  value={studentSelected && studentSelected.email}/>
            <br/>
            <label>Age:</label>
            <br/>
            <input type='text' className='form-control' name='age' onChange={handleChange}
                  value={studentSelected && studentSelected.age}/>
            <br/>
          </div>
        </ModalBody>
        <ModalFooter>
          <button className='btn btn-primary' onClick={() => requestPut()}>Edit</button>{"   "}
          <button className='btn btn-danger' onClick={() => openCloseModalEdit()}>Cancel</button>
        </ModalFooter>
        </Modal>

        <Modal isOpen={modalDelete}>
          <ModalBody>
            Dou you really want delete this student: {studentSelected && studentSelected.name} ?
          </ModalBody>
          <ModalFooter>
            <button className='btn btn-danger' onClick={()=>requestDelete()}>Yes</button>
            <button className='btn btn-secondary' onClick={()=>openCloseModalDelete}>No</button>
          </ModalFooter>
        </Modal>
      </div>
    );
}

export default App;
