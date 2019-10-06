import React from "react";
import * as moment from 'moment';
import { inject } from "mobx-react";
import { Form, Rate, Input, Button, DatePicker, Alert, message } from 'antd';
import { Formik } from "formik";
import * as yup from "yup";

const { TextArea } = Input;

const desc = ['terrible', 'bad', 'normal', 'good', 'wonderful'];

@inject("ReviewStore")
class ReviewForm extends React.Component {
    render() {
        const {
            restaurantId,
            ReviewStore,
            cb,
        } = this.props;

        return (
            <Formik
                initialValues={{
                    rating: 0,
                    text: "",
                    dateOfVisit: moment()
                }}
                validationSchema={yup.object().shape({
                    text: yup
                        .string()
                        .min(3, "please describe the your visit")
                        .max(200)
                        .required(),
                    rating: yup
                        .number()
                        .min(1, "please rate your visit")
                        .required(),
                })}
                onSubmit={async (values, formikBag) => {
                    if (values.dateOfVisit.isAfter(moment())) {
                        formikBag.setFieldError("dateOfVisit", "please select a valid date of visit");
                        formikBag.setSubmitting(false);
                        return;
                    }
                    formikBag.setStatus();

                    try {
                        await ReviewStore.Create(restaurantId, {
                            rating: values.rating,
                            text: values.text,
                            dateOfVisit: values.dateOfVisit.format()
                        });
                        formikBag.setSubmitting(false);
                        cb();
                    } catch (error) {
                        message.error("An error ocurred!");
                        formikBag.setSubmitting(false);
                    }
                }}
                render={({ values, handleChange, handleBlur, touched, errors, isSubmitting, status, handleSubmit, setFieldValue, setFieldError }) => (
                    <Form onSubmit={handleSubmit}>
                        <Form.Item
                            help={touched.rating && errors.rating ? errors.rating : ""}
                            validateStatus={touched.rating && errors.rating ? "error" : undefined}
                        >
                            <span>
                                <Rate
                                    name="rating"
                                    tooltips={desc}
                                    value={values.rating}
                                    onChange={(value) => setFieldValue("rating", value)}
                                />
                                {values.rating ? <span className="ant-rate-text">{desc[values.rating - 1]}</span> : ''}
                            </span>
                        </Form.Item>
                        <Form.Item
                            help={touched.text && errors.text ? errors.text : ""}
                            validateStatus={touched.text && errors.text ? "error" : undefined}
                        >
                            <TextArea
                                name="text"
                                rows={4}
                                value={values.text}
                                onChange={handleChange}
                                onBlur={handleBlur}
                            />
                        </Form.Item>
                        <Form.Item
                            help={touched.dateOfVisit && errors.dateOfVisit ? errors.dateOfVisit : ""}
                            validateStatus={touched.dateOfVisit && errors.dateOfVisit ? "error" : undefined}>
                            <DatePicker
                                name="dateOfVisit"
                                value={values.dateOfVisit}
                                placeholder="Select date of visit"
                                onChange={(date, dateString) => {
                                    if (date.isAfter(moment())) {
                                        setFieldError("dateOfVisit", "please select a valid date of visit");
                                    }
                                    setFieldValue("dateOfVisit", date);
                                }}
                            />
                        </Form.Item>
                        <Form.Item>
                            <Button
                                type="primary"
                                htmlType="submit"
                            >
                                Submit
                            </Button>
                            {isSubmitting &&
                                <img style={{ marginLeft: "10px" }} alt="loader" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                            }
                        </Form.Item>
                        {status &&
                            <Alert style={{ marginTop: "10px" }} type="error">{status}</Alert>
                        }
                    </Form>
                )}
            />
        )
    }
}

export default ReviewForm;